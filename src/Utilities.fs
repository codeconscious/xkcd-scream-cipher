module Utilities

open System

let flip map : Map<'a, 'b> =
    Map.fold (fun acc k v -> Map.add v k acc) Map.empty map

// Merge two maps with the values of the 'primary' one taking precedence.
let merge secondary primary : Map<'a, 'b> =
    Map.fold (fun acc k v -> Map.add k v acc) secondary primary

let caseInsensitiveEquals (x: string) (y: string) : bool =
    x.Equals(y, StringComparison.InvariantCultureIgnoreCase)

let groupByValues items : seq<'a * 'b list> =
    items
    |> Seq.map (fun (KeyValue(k, vs)) -> vs, k)
    |> Seq.groupBy fst
    |> Seq.map (fun (op, pairs) ->
        let keys = pairs |> Seq.map snd |> Seq.toList
        op, keys)
