# xkcd's Scream Cipher

This is an implementation of popular web comic [xkcd](https://xkcd.com/)'s so-called "[scream cipher](https://xkcd.com/3054/)" written in F#.

![xkcd #3054: Scream Cipher](https://imgs.xkcd.com/comics/scream_cipher.png)

## Summary

- Replaces all letters in strings with `A`'s containing various diacritics (e.g., `A̋`, `A̧`, `A̤`)
- Decodes such strings, reverting them back to their original letters
- Non-supported characters (digits, punctuation, etc.) are not converted and are included as-is
- Multiple strings can be passed at once

## Requirement

.NET 9 runtime

## Usage

```
dotnet run -- [--encode|--decode|--test] input1 optionalInput2
```

> [!NOTE]
> `--` is needed after `dotnet run` to signal that the arguments are for this application and not the `dotnet` command.

## Errata

- My corresponding blog post: [xkcd Scream Cipher](https://codeconscious.github.io/2025/02/23/xkcd-scream-cipher.html)
- Special thanks to [FrostBird347](https://github.com/FrostBird347), whose [JavaScript implementation](https://gist.github.com/FrostBird347/e7c017d096b3b50a75f5dcd5b4d08b99) saved considerable time and trouble gathering the necessary variants of `A`.
