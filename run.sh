#!/bin/bash

export PATH="$PATH:/c/Program Files/dotnet"

ROOT="$(cd "$(dirname "$0")" && pwd)"

kill_port() {
  local pid
  pid=$(lsof -ti tcp:"$1" 2>/dev/null)
  if [ -n "$pid" ]; then
    echo "Killing process on port $1 (PID $pid)"
    kill "$pid"
  else
    echo "Nothing running on port $1"
  fi
}

cmd_start() {
  echo "=== Starting Backend ==="
  cd "$ROOT/backend" && dotnet run --project BerlinClock.Api &
  BACKEND_PID=$!

  echo "=== Starting Frontend ==="
  cd "$ROOT/frontend" && npm run dev &
  FRONTEND_PID=$!

  echo "Backend PID: $BACKEND_PID | Frontend PID: $FRONTEND_PID"
  echo "Press Ctrl+C to stop both"
  wait
}

cmd_stop() {
  echo "=== Stopping Services ==="
  kill_port 5000
  kill_port 5001
  kill_port 5173
}

cmd_test() {
  cmd_stop

  echo ""
  echo "=== Backend Tests ==="
  cd "$ROOT/backend" && dotnet test

  echo ""
  echo "=== Frontend Type Check ==="
  cd "$ROOT/frontend" && npm run check
}

cmd_build() {
  echo "=== Building Backend ==="
  cd "$ROOT/backend" && dotnet build -c Release

  echo ""
  echo "=== Building Frontend ==="
  cd "$ROOT/frontend" && npm run build
}

case "$1" in
  start) cmd_start ;;
  stop)  cmd_stop ;;
  test)  cmd_test ;;
  build) cmd_build ;;
  *)
    echo "Usage: $0 {start|stop|test|build}"
    exit 1
    ;;
esac
