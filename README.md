# Advent of Code

## Options

> --day, -d value :  Sets the day to run. Valid values are -1 (Today), 0 (All days) and 1-25 (The specified day) 

> --year, -y value :  Sets which years AoC to run. 

> --useExample, -e true/false :  Run the day(s) using example data (true) or real data (false). 
## Examples
Run all days for the latest supported year using real data. 
> dotnet run

Run all days for year 2020 using real data. 
> dotnet run --year 2020

Run the current day for the latest supported year using example data
> dotnet run --useExample true --day -1 

Run the current day for the year 2020 using real data
> dotnet run --year 2020 --day -1 

Run the fourth for the latest supported year day using real data
> dotnet run --useExample false --day 4