# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com)
and this project adheres to [Semantic Versioning](http://semver.org).

## [1.3.0] - 2024-04-02
### Added
- Add the `ICompressor` interface for file compression / decompression. Compression often leads to a 50-75% file reduction.
- Add `DeflateCompressor` for compressing files using Deflate compression.
- Add `GZipCompressor` for compressing files using GZip compression.
- Add `ISaver.encoding` to make `ISaver`'s encoding publically available.
- Add `IEnumerableSaver.encoding` to make `IEnumerableSaver`'s encoding publically available.
- Add `ILoader.encoding` to make `ILoader`'s encoding publically available.
- Add `IEnumerableLoader.encoding` to make `IEnumerableLoader`'s encoding publically available.

### Changed
- Replace `UpdateSynchronizer` with `ManualSynchronizer`. Due to the unreliable nature of destructors, `UpdateSynchronizer`'s subscription to `UpdaterService` may not always be removed, leaking code with the ability to cause unintended consequences. You must now process `ManualSynchronizer` queue manually by calling `ManualSynchronizer.ProcessQueue()`.
- Change `Timer` to return its full timestep. Previously it would return a rounded-off timestep for convenience, but if this rounding is desired, it should be done on the receivers end.

### Removed
- Remove `UpdaterService` as `Cubusky.Core` should contain no scripts running in the background, and with the replacement from `UpdateSynchronizer` to `ManualSynchronizer`, this service is no longer needed.

## [1.2.1] - 2024-03-21
### Added
- Add `PasswordAttribute` to mask a string field in the inspector for screen share. Note that it doesn't actually encrypt or otherwise protect your string.
- Add `HelpBoxAttribute` to decorate your property with a useful help box.

## [1.2.0] - 2024-02-23
### Added
- Add `ISaver` to allow saving data to a single location.
- Add `ILoader` to allow loading data from a single location.
- Add `IEnumerableSaver` to allow saving a collection of data to multiple locations.
- Add `IEnumerableLoader` to allow loading a collection of data from multiple locations.
- Add `ISaverLoader` to conveniently implement and reference `ISaver` & `ILoader` through the same interface.
- Add `IEnumerableSaverLoader` to conveniently implement and reference `IEnumerableSaver` & `IEnumerableLoader` through the same interface.
- Add `IFileSaver` to allow saving data to a file.
- Add `IFileLoader` to allow loading data from a file.
- Add `IDirectorySaver` to allow saving (a collection of) data to a directory under random file names.
- Add `IDirectoryLoader` to allow loading a collection of data from a directory.
- Add `ITempSaver` to allow saving data to memory.
- Add `ITempLoader` to allow loading data from memory.
- Add `PathAttribute` to serialize `string` as a valid path.
- Add `UriAttribute` to serialize `string` as a valid `Uri`.
- Add `ApplicationPath` to select an `Application.*Path` via `Enum`.
- Add `UnityPath` as an abstract class for returning a path connected to one of Unity's `Application.*Path`.
- Add `TimeMode` to select a `Time.(fixed)(Unscaled)(Delta)(Time)` via `Enum`.
- Add an optional `Type[] types` to `ReferenceDropdown` to allow for creating a dropdown for generic types.
    Invalid: `[SerializeReference, ReferenceDropdown] public ISaver<Vector3> saver; // SerializeReference cannot serialize generics.`
    Solution: `[SerializeReference, ReferenceDropdown(typeof(ISaver<Vector3>))] public object saver;`
- Add `UpdaterService` which provides callbacks to `onUpdate`, `onLateUpdate` and `onFixedUpdate`.
- Add `Timer` implementation that can be serialized by the inspector. A Timer can be used to call an event every x amount of seconds. This is useful for e.g. optimizing operations outside of Unity's update loop.
- Add `UpdateSynchronizer` to allow synchronizating with Unity's Update loop. `Timer` example: `public Timer timer = new() { SynchronizingObject = new UpdateSynchronizer() };`

### Fixed
- Fixed some mixed value errors:
    - `ReferenceDropdownAttribute` could get stuck in an infinite loop when changing a mixed value. It now correctly disallows multi-value editing.
    - `GuidAttribute` would change its `Guid` when selecting multiple values at once. It now allows you to edit, but doesn't automatically destroy your `Guid`.

## [1.1.0] - 2024-01-07
- Add `ReferenceDropdownAttribute` to allow for selectable references in `UnityEngine.Object`s. Useful for allowing the use of interfaces in code.

## [1.0.0] - 2024-01-01
- Add documentation.

## [0.9.0-pre] - 2023-12-30
### Added
- Add documentation, license and changelog url.
- Add a UIElement `TimeSpanField` that may be used to draw a `TimeSpan` in the inspector.
- Add a `TimeSpanAttribute` to allow `long` to be drawn as `TimeSpan` in the inspector. Use like so: `[TimeSpan] public long timeSpan;`
- Add an `OfTypeAttribute` to allow `Object`s to specify which types they need to be of. Use like so: `[OfType(typeof(IPointerClickHandler), typeof(ISubmitHandler))] public Selectable pointClickSubmitHandler;`

### Removed
- Remove unused `SerializedPropertyExtensions` before version 1.0 as their integrety cannot be verified at this time.

## [0.1.0-exp] - 2023-05-15
### This is the first release of *Cubusky Core*.
Cubusky Core contains reusable code spanning across the Cubusky organization including serialization features and utility functions. If you are creating a custom package based on the Cubusky organization, using Cubusky Core will save you time.