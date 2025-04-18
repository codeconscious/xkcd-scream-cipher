module Utilities

open System

let flipMap map : Map<'a, 'b> =
      Map.fold (fun m k v -> m.Add(v, k)) Map.empty map

let combineMaps map1 map2 :  Map<'a,'b> =
    Map.fold (fun acc key value -> Map.add key value acc) map1 map2

let caseInsensitiveEquals (x: string) (y: string) : bool =
    x.Equals(y, StringComparison.InvariantCultureIgnoreCase)
