# config.yaml
Used to configure the overall behaviour of the application:

## Sample Configuration

```yaml
output_dir: C:\Dev\personal\pihole-adlist\lists\
local_repo: ...
db_connection_string: "..."
list_generation:
  generate_category_lists: true
  generate_combined_lists: true
  category_all: true
  category_safe: true
  combined_all: true
  combined_safe: true
  insert_batch_size: 1000
  update_batch_size: 5000
development:
  enabled: false
  use_cached_lists: true
  capture_responses: false
  cached_response_dir: C:\Dev\personal\_blocklists
  capture_response_dir: C:\Dev\personal\_blocklists
block_lists:
  - name: suspicious
    enabled: true
    entries:
      - url: https://someonewhocares.org/hosts/zero/hosts
        strict: true
whitelist:
  regex:
    - .*\.whatsapp\.(net|com)$
    - .*?\.gvt(\d{1,})\.com
  exact:
    - fonts.gstatic.com
```

### output_dir
More to come...

### local_repo
More to come...

### db_connection_string
More to come...

### list_generation
More to come...

### development
More to come...

### block_lists
More to come...

### whitelist
More to come...
