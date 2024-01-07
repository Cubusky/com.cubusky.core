# About Cubusky Core
Cubusky Core contains core functionality spanning across the Cubusky organization.

## Installing Cubusky Core
To install this package, follow the instructions on the [Package Manager documentation](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

## Requirements
This version of Cubusky Core is compatible with the following versions of the Unity Editor:
- 2022.3 and later (recommended)

## Known Limitations
Cubusky Core `1.1.0` includes the following known limitations:
- TimeSpanField in the UIBuilder allows for weird string inputs. This may be improved using [UxmlElementAttribute](https://docs.unity3d.com/2023.2/Documentation/ScriptReference/UIElements.UxmlElementAttribute.html) in a later Unity version.
- The OfType attribute does not single out objects shown in the [Object Picker](https://docs.unity3d.com/Manual/search-advanced-object-picker.html).