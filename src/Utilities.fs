module Utilities

open System
open System.Collections.Generic

let flip (map: Map<'k, 'v>) : Map<'v, 'k> =
    Map.fold
        (fun acc k v -> Map.add v k acc)
        Map.empty map

let merge secondary primary : Map<'k, 'v> =
    Map.fold
        (fun acc k v -> Map.add k v acc)
        secondary
        primary // Takes precedence, overwriting as needed.

let caseInsensitiveEquals (first: string) (second: string) : bool =
    first.Equals(second, StringComparison.InvariantCultureIgnoreCase)

let groupByValues (pairs: KeyValuePair<'k, 'v> seq) : ('v * 'k seq) seq =
    pairs
    |> Seq.groupBy _.Value
    |> Seq.map (fun (v, pairs) -> v, pairs |> Seq.map _.Key)

let toNestedPairs (innerSeparator: string)
                  (outerSeparator: string)
                  (tuples: ('k * string seq) seq)
                  : string =
    tuples
    |> Seq.map (fun (_, vs) -> vs |> String.concat innerSeparator)
    |> String.concat outerSeparator
