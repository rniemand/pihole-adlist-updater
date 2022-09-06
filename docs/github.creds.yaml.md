# github.creds.yaml
The `github.creds.yaml` file is used to configure the credentials used for committing and pushing any updates to the generated adlists.

You can rename the sample `github.creds.sample.yaml` to `github.creds.yaml` for a starting point, the final file should look something like this:

```yaml
username: user
token: token_here
author_name: Richard Niemand
author_email: email@address.com
app_name: PiHoleUpdater
```

All other configuration can be found [here](./config.yaml.md).

## Configuration Keys
- `username` - Your GitHub username.
- `token` - your GitHub access token (can be managed [here](https://github.com/settings/tokens))
- `author_name` - the committing author name
- `author_email` - the committing author email
- `app_name` - unused for now - the name of the committing application
