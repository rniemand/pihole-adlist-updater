# PiHole Adlist Updater

Helper project used to generate and update my custom [Pi-hole](https://pi-hole.net/) blocklists ([found here](https://github.com/rniemand/pihole-adlist)).

These lists are built off a collection of other blocklists, all used lists are [listed and accredited here](./blocklists.md).

## Docker Image

Can be grabbed [here](https://hub.docker.com/repository/docker/niemandr/pihole-updater).

- Repository: `niemandr/pihole-updater`
- Paths
  - (Optional): `/app/appsettings.json`
  - (Required): `/app/config.yaml`
  - (Required): `/app/github.creds.yaml`
  - (Required): `/app/service.state.json` - `RW`
  - (Required): `/repos/pihole-adlist`
