#!/bin/bash
apt update
apt install -y libpq-dev
apt install -y pkg-config

Rscript packages.r