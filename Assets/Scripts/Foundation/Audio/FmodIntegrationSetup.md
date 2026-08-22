# FMOD integration

Import the official FMOD for Unity package into this project. The package defines `FMOD_PRESENT`, which activates `FmodOneShotPlayer` and `FmodMusicPlayer`.

Do not add bank paths to these scripts. In the Inspector, assign event references directly:

- `FmodOneShotPlayer`: assign an event, click **Refresh Event Parameters**, then set the exposed parameter values.
- `FmodMusicPlayer`: assign one music event with an authored loop region, click **Refresh Event Parameters**, then set the exposed parameter values. Call `PlayLooped` to start it; FMOD preserves the authored loop points.
- `AudioManager`: assign the one-shot and music-player components, then optionally assign a Scene Start Event. Its event starts automatically with the scene. Each event's inspector has a **Refresh Event Parameters** button that exposes all of its current local FMOD parameters.
