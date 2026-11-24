"""
Makes every sound and the music track for Assignment 04.

Nothing here is recorded or downloaded. Each sound is built from sine waves, a little noise and an
envelope (how loud it is over time). Run it from the repo folder:

    python Docs/make_audio.py

and it writes the .wav files into Assets/Audio/.
"""
import wave
import numpy as np

SR = 44100
OUT = "Assets/Audio/"
rng = np.random.default_rng(464)   # same noise every time, so the files do not change between runs


def write(name, x, peak=0.8):
    x = x / max(1e-9, np.max(np.abs(x))) * peak
    data = (np.clip(x, -1, 1) * 32767).astype("<i2")
    with wave.open(OUT + name, "wb") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(SR)
        w.writeframes(data.tobytes())
    print(name, round(len(x) / SR, 2), "s")


def tone(freq, dur, decay=6.0, attack=0.004, harmonics=(1.0, 0.3, 0.12)):
    """A plucked note: a few sine harmonics and a loudness that dies away."""
    t = np.arange(int(dur * SR)) / SR
    wave_ = sum(a * np.sin(2 * np.pi * freq * (k + 1) * t) for k, a in enumerate(harmonics))
    env = np.minimum(1.0, t / attack) * np.exp(-decay * t)
    return wave_ * env


def place(total, parts):
    """Put each (start_seconds, samples) into one buffer."""
    out = np.zeros(int(total * SR))
    for start, x in parts:
        i = int(start * SR)
        out[i:i + len(x)] += x[:len(out) - i]
    return out


def smooth_noise(n, width):
    return np.convolve(rng.standard_normal(n), np.ones(width) / width, mode="same")


# ---------------------------------------------------------------- the sound effects
def pickup():
    # two quick notes going up, like a coin
    write("pickup.wav", place(0.4, [(0.0, tone(1318.5, 0.3, 9)), (0.07, tone(1975.5, 0.3, 9))]), 0.7)


def hit():
    n = int(0.45 * SR)
    t = np.arange(n) / SR
    noise = smooth_noise(n, 12) * np.exp(-16 * t)
    sweep = 150 * np.exp(-9 * t) + 45                     # a low thump that falls in pitch
    thump = np.sin(2 * np.pi * np.cumsum(sweep) / SR) * np.exp(-8 * t)
    write("hit.wav", noise * 0.9 + thump * 1.1, 0.85)


def button():
    n = int(0.09 * SR)
    t = np.arange(n) / SR
    click = np.sin(2 * np.pi * 1100 * t) * np.exp(-55 * t) + 0.3 * rng.standard_normal(n) * np.exp(-300 * t)
    write("button.wav", click, 0.6)


def win():
    notes = [523.25, 659.25, 783.99, 1046.5]
    parts = [(0.12 * i, tone(f, 1.3, 2.6)) for i, f in enumerate(notes)]
    parts += [(0.5, tone(f, 1.0, 3.0) * 0.5) for f in (523.25, 659.25, 783.99)]   # the last chord rings
    write("win.wav", place(1.8, parts), 0.8)


def lose():
    notes = [392.0, 311.13, 261.63, 196.0]
    parts = [(0.3 * i, tone(f, 1.0, 3.2, harmonics=(1.0, 0.55, 0.3, 0.15))) for i, f in enumerate(notes)]
    write("lose.wav", place(1.7, parts), 0.8)


def checkpoint():
    write("checkpoint.wav", place(0.6, [(0.0, tone(880, 0.5, 7)), (0.1, tone(1318.5, 0.5, 7))]), 0.6)


# ---------------------------------------------------------------- sounds that live in the level (loops)
def guard_hum():
    # 2 seconds. Every frequency has a whole number of cycles in 2 s, so the loop has no click.
    t = np.arange(int(2 * SR)) / SR
    hum = (np.sin(2 * np.pi * 110 * t) + 0.8 * np.sin(2 * np.pi * 112 * t)
           + 0.3 * np.sin(2 * np.pi * 220 * t) + 0.15 * np.sin(2 * np.pi * 330 * t))
    hum *= 0.8 + 0.2 * np.sin(2 * np.pi * 4 * t)           # a slow wobble, 4 times a second
    write("guard_hum.wav", hum, 0.6)


def goal_beacon():
    # 3 seconds, again whole cycles: 440 Hz x 3 s = 1320 cycles and so on
    t = np.arange(int(3 * SR)) / SR
    chime = np.sin(2 * np.pi * 440 * t) + 0.6 * np.sin(2 * np.pi * 660 * t) + 0.25 * np.sin(2 * np.pi * 880 * t)
    chime *= 0.65 + 0.35 * np.sin(2 * np.pi * 2 * t)       # it pulses twice a second
    write("goal_beacon.wav", chime, 0.55)


# ---------------------------------------------------------------- the music
def music():
    bpm = 120
    beat = 60 / bpm
    bar = 4 * beat
    chords = [  # four bars each: A minor, F, C, G
        ([57, 60, 64], 45), ([53, 57, 60], 41), ([48, 52, 55], 36), ([55, 59, 62], 43),
    ]
    bars = 16
    total = bars * bar
    n = int(total * SR)

    def hz(m):
        return 440.0 * 2 ** ((m - 69) / 12)

    out = np.zeros(n)
    for c, (notes, root) in enumerate(chords):
        for b in range(4):
            start = (c * 4 + b) * bar
            # a soft pad, one chord per bar
            for m in notes:
                t = np.arange(int(bar * SR)) / SR
                env = np.minimum(1, t / 0.6) * np.minimum(1, (bar - t) / 0.5)
                pad = np.sin(2 * np.pi * hz(m) * t) + 0.25 * np.sin(2 * np.pi * hz(m) * 2 * t)
                i = int(start * SR)
                out[i:i + len(t)] += 0.10 * pad * env
            # a plucked arpeggio on every eighth note
            pattern = [0, 1, 2, 1, 0, 1, 2, 1]
            for k, idx in enumerate(pattern):
                m = notes[idx] + 12
                i = int((start + k * beat / 2) * SR)
                x = tone(hz(m), 0.6, 5.5, harmonics=(1.0, 0.2))
                out[i:i + len(x)] += 0.11 * x[:max(0, n - i)][:len(out[i:i + len(x)])]
            # a bass note on each beat
            for k in range(4):
                i = int((start + k * beat) * SR)
                x = tone(hz(root), 0.5, 4.0, harmonics=(1.0, 0.15))
                out[i:i + len(x)] += 0.30 * x[:max(0, n - i)][:len(out[i:i + len(x)])]
            # a quiet kick on beats 1 and 3, and a hat on every off-beat
            for k in (0, 2):
                i = int((start + k * beat) * SR)
                tt = np.arange(int(0.2 * SR)) / SR
                kick = np.sin(2 * np.pi * np.cumsum(110 * np.exp(-25 * tt) + 42) / SR) * np.exp(-14 * tt)
                out[i:i + len(kick)] += 0.28 * kick[:len(out[i:i + len(kick)])]
            for k in range(8):
                if k % 2 == 1:
                    i = int((start + k * beat / 2) * SR)
                    tt = np.arange(int(0.05 * SR)) / SR
                    hat = np.diff(smooth_noise(len(tt) + 1, 2)) * np.exp(-70 * tt)
                    out[i:i + len(hat)] += 0.5 * hat[:len(out[i:i + len(hat)])]

    # a very short fade at both ends, so the loop does not click where it restarts
    fade = np.linspace(0, 1, int(0.02 * SR))
    out[:len(fade)] *= fade
    out[-len(fade):] *= fade[::-1]
    write("music.wav", out, 0.55)


if __name__ == "__main__":
    pickup(); hit(); button(); win(); lose(); checkpoint()
    guard_hum(); goal_beacon()
    music()
