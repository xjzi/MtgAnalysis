#!/bin/bash

# These two packages are necessary for RPostgres.
apt update
apt install -y libpq-dev
apt install -y pkg-config