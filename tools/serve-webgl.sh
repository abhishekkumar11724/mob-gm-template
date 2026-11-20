#!/usr/bin/env bash
# Serve the WebGL build on the local network so it opens in a desktop or phone browser.
# Usage: tools/serve-webgl.sh [port]    (default 8080)
set -euo pipefail

port="${1:-8080}"
root="$(cd "$(dirname "$0")/.." && pwd)/sample-game/Builds/WebGL"

if [[ ! -f "$root/index.html" ]]; then
  echo "No WebGL build at $root. Build first:" >&2
  echo "  Unity -batchmode -nographics -quit -projectPath sample-game -buildTarget WebGL \\" >&2
  echo "        -executeMethod Nimbus.Build.BuildRunner.BuildWebGL [-development]" >&2
  exit 1
fi

lan_ip="$(hostname -I 2>/dev/null | awk '{print $1}')"
echo "Serving $root"
echo "  This machine: http://localhost:${port}"
[[ -n "$lan_ip" ]] && echo "  Phone (same Wi-Fi): http://${lan_ip}:${port}"

exec python3 -m http.server "$port" --bind 0.0.0.0 --directory "$root"
