# PlotFromFile

This application is written in `F#` and use the plotting library
[plotly.NET](https://plotly.net/).

## Usage

Use the argument `--searchtext` to specify a research regex. Two items are
needed and you must use named capture groups.

The first capture group is `(?<D>...your regex here)` (D for date) and is used
to capture the log timestamp.

The second capture group is `(?<M>...your regex here)` (M for match) and is used
for capturing the value your want to compare.

An example regex to capture the timestamp of the second battery level in
this example log line:

```txt
2022-08-03 08:29:19.712 -06:00 [DBG] trackerpa23wfixbatteryPercentage-gBatteryPercent=29, BatteryPercent=30 ,Tracker_get_chr_state() 1
```

Use this regex `(?<D>^.*-\d+:\d+)` to get the timestamp.
Use this regex `(?<M>\d+)` to get the battery value.

But these need to be combined into a single regex like this to actually work:

`(?<D>^.*-\d+:\d+).*, BatteryPercent=(?<M>\d+)`

And can set which files you want using the `--file` option followed by a list of
files.

Example usage of the application:

```powershell
PlotFromFile --searchtext "(?<D>^.*-\d+:\d+).*, BatteryPercent=(?<M>\d+)" --file 'C:\Users\Derek Lomax\AppData\Roaming\fm-cli\debug-logs-COM33.20220803.txt'
```

### More examples

BelleX_server log

```powershell
PlotFromFile --searchtext "(?<D>^.*-\d+:\d+).*batteryPercent.:(?<M>\d+)" --file 'C:\Users\Derek Lomax\AppData\Roaming\BelleX_Server\BelleX_OTA_Logs\alQWGVDL.txt'
```

BX device log

```powershell
--searchtext " (?<D>\d{4}-\d+-\d+ \d+:\d+:\d+) .*battery_level.*?(?<M>\d+)$"  --file B:\debugging\2022-12-12_dealer_audio_played_without_seeming_trigger\data\freeus_app\errlogpool.txt
```
