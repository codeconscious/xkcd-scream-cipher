module ArgValidation

open System
open System.Collections.Generic
open Utilities
open FsToolkit.ErrorHandling

type Operation = Encode | Decode | Test

type ValidatedArgs =
    { Operation: Operation
      Inputs: string array }

let supportedFlags =
    // A dictionary, currently, preserves the order of added items,
    // whereas F#'s native Map does not.
    let od = Dictionary<string, Operation>()
    od.Add("--encode", Encode)
    od.Add("-e", Encode)
    od.Add("--decode", Decode)
    od.Add("-d", Decode)
    od.Add("--test", Test)
    od.Add("-t", Test)
    od

let flagSummary =
    let flags =
        supportedFlags
        |> groupByValues
        |> Seq.map (fun opGroup -> String.Join(", ", snd opGroup))
        |> String.concat "; "

    $"""Supported flags: %s{String.Join(", ", flags)}"""

let private validateArgCount (args: string array) =
    let instructions = $"Supply an operation flag and at least one string to convert.\n%s{flagSummary}"

    match args.Length with
    | 0 -> Error $"No arguments were passed. %s{instructions}"
    | 1 -> Error $"Insufficient arguments. %s{instructions}"
    | _ -> Ok args

let private validateFlag (flag: string) =
    if supportedFlags.ContainsKey flag
    then Ok supportedFlags[flag]
    else Error $"Unsupported flag \"%s{flag}\". %s{flagSummary}."

let private validateInputs (inputs: string array) =
    if inputs.Length = 0
    then Error "No inputs to convert were passed."
    else Ok (inputs |> Array.map _.ToUpperInvariant())

let validate (rawArgs: string array) =
    result {
        let! args = validateArgCount rawArgs
        let flag, inputs = args[0], args[1..]
        let! operation = validateFlag flag
        let! inputs' = validateInputs inputs
        return { Operation = operation; Inputs = inputs' }
    }

