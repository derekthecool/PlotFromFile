open System
open System.IO
open System.Text.RegularExpressions
open CommandLineArgumentParser

open Plotly.NET
open Plotly.NET.ImageExport

Defaults.DefaultTemplate <- ChartTemplates.dark

let parsedArgs = parseArgs (Environment.GetCommandLineArgs())

let files = parsedArgs.TryFind "file"

let searchtext =
    match (parsedArgs.TryFind "searchtext") with
    | Some y -> y |> Seq.head
    | None -> ""

printfn "%A" searchtext

files
|> fun filesOption ->
    match filesOption with
    | Some x ->
        x
        |> Seq.iter (fun item ->
            printfn "Processing file: %s" item

            if File.Exists(item) then
                printfn "file exists"
                let lines = File.ReadLines(item)

                let matching =
                    lines
                    |> Seq.filter (fun text -> Regex.IsMatch(text, searchtext))
                    |> Seq.map (fun text ->
                        let m = Regex.Match(text, searchtext)
                        (m.Groups["D"].Value, m.Groups["M"].Value))
                let xAxisTimeData = matching |> Seq.map(fun (a,_)->
                    DateTime.Parse(a))
                let yAxisUserData = matching |> Seq.map(fun (_,b)->int b)

                let title = "Plot for " + Path.GetFileNameWithoutExtension(item)
                Chart.Point(xAxisTimeData, yAxisUserData)
                |> Chart.withTitle title
                |> Chart.withXAxisStyle "Timespan"
                |> Chart.withYAxisStyle "Values"
                |> Chart.show

                printfn "x axis: %A" xAxisTimeData
                printfn "y axis: %A" yAxisUserData
            else
                printfn "File not found: %s" item)


    | None -> printfn "No files given in the --file argument"
