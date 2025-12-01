# Advent of Code

Setup a new year with

> dotnet run --setupYear 2021 # Replace 2021 with the year you want to setup.

## Options

> --day, -d value : Sets the day to run. Valid values are -1 (Today), 0 (All days) and 1-25 (The specified day)

> --year, -y value : Sets which years AoC to run.

> --createDay, -c : Use this flag to create a file for the given day

> --useExample, -e : Run the day(s) using example data.

> --skipMissing, -x : Don't create days that haven't been created for a year.

## Examples

Run all days for the latest supported year using real data.

> dotnet run

Run all days for year 2020 using real data.

> dotnet run --year 2020

Run the current day for the latest supported year using example data

> dotnet run --useExample --day -1

Create a file for a given day

> dotnet run --day 6 --createDay

Run the current day for the year 2020 using real data

> dotnet run --year 2020 --day -1

Run the fourth for the latest supported year day using real data

> dotnet run --day 4

Create a file for the current day for the year 2025. Includes fetching input (real, not example)

> dotnet run --year 2025 --createDay
