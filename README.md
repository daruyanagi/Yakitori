# Yakitori

Windows 10 にはスクリーンショットを撮るためのキーボードショートカットがいくつか用意されています。

- ［PrintScreen］：デスクトップのスクリーンショットをクリップボードへ保存
- ［Windows］＋［PrintScreen］：デスクトップのスクリーンショットを Pictures\Screenshots に保存
- ［Alt］＋［PrintScreen］：アクティブウィンドウのスクリーンショットをクリップボードへ保存
- ［Windows］＋［Shift］＋［S］：デスクトップの一部を矩形選択してクリップボードへ保存

いずれも便利ですが、

- 覚えにくい
- キーボードがない環境で使いにくい（デバイスのボタンで使えることもあります）
- クリップボードへ保存されたイメージをファイルにしたい
- カウントダウン撮影機能ぐらいはほしい
- ちゃんと保存されたのか分かりにくい

といった不満も感じます。「Yakitori」はその一部を解消するために開発された、割と単純なユーティリティです。

![](https://cdn-ak.f.st-hatena.com/images/fotolife/d/daruyanagi/20180610/20180610121919.png)

- ジャンプリストから簡単に使えるようにしました
- クリップボードへ送られたイメージをファイルへ保存できます
- カウントダウンができます
- 撮影時にサウンドでお知らせします

コマンドラインでも使えます（小文字のオプションはクリップボードへの転送のみを行います）
/d のみ、大文字の /D にするとファイル保存も行います（［Windows］＋［PrintScreen］の実行と等価）

|オプション|効果|
|--|--|
|/d|デスクトップ全体|
|/a|アクティブウィンドウ|
|/r|矩形選択|
|/s|クリップボードへ保存されたイメージを Pctures\Screenshots に保存|
|/o|Ptures\Screenshots を開く|
