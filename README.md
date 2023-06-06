# PatrolHelicopterFix

For Oxide / Umod Rust Servers - PatrolHelicopterFix modifies the patrol helicopter's path to exclude the specified monuments or markers. The patrol helicopter will no longer target these ignored locations during its patrol.

## Features

- Ignore specific monuments via markers defined in a configuration file.
- Modify the patrol helicopter's path to exclude ignored monuments.
- Improved targeting behavior for the patrol helicopter.

## Installation

1. Make sure you have the Oxide / Umod installed on your Rust server.
2. Place the downloaded plugin file (`PatrolHelicopterFix.cs`) into the `oxide/plugins` directory of your server.
3. Configure the `PatrolHelicopterFix.json` file located in your `oxide/config` directory. **Monument markers are case sensitive!**

## Configuration

You can modify this file to specify the monuments or markers that should be ignored by the patrol helicopter.
**Once again, please note that monuments or their respective markers are case sensitive!**

Example configuration:

```json
{
  "IgnoredMonuments": [
    "Giant Excavator Pit",
    "Water Treatment Plant",
    "*"
  ]
}
```

### Known Issues
The Patrol Helicopter will appear to aimlessly roam the map if there are less than three monuments or markers. Due to a lack of patrol paths, the plugin will compensate by inserting random corners into the patrol path navmesh.

### Support
If you encounter any issues or have any questions or suggestions, please feel free to open an issue on the GitHub repository.

### License
This plugin is released under the MIT License.
