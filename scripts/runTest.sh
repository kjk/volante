#!/bin/bash

set -o nounset
set -o errexit
set -o pipefail

make tests
mono ./bin/rel/Tests.exe $@
