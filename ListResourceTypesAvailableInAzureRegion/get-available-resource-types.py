import json
import sys
import subprocess

if len(sys.argv) < 2:
    print("Usage: python process_providers.py <region_name>")
    sys.exit(1)

region_name = sys.argv[1]

# Run the az provider list command and capture its output
result = subprocess.run(["az.cmd", "provider", "list", "--output", "json"], capture_output=True, text=True)
providers = json.loads(result.stdout)

for provider in providers:
    namespace = provider["namespace"]
    for resource_type in provider["resourceTypes"]:
        if "locations" in resource_type and region_name in resource_type["locations"]:
            print(f"{namespace}/{resource_type['resourceType']}")