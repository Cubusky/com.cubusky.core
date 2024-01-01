# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com)
and this project adheres to [Semantic Versioning](http://semver.org).

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