# XUnity.AutoTranslator.DeepSeek

Translator that uses DeepSeek and a custom prompt to translate the game in parallel.

## Installation instructions

1. Build the assembly
2. Install [XUnity.AutoTranslate]() into your game as normal
3. Place XUnity.AutoTranslatorDeepSeek.dll into `<GameDir>/ManagedData/Translators` (Should see the other translators)
4. Update your Config.ini in `<GameDir>/AutoTranslator`
	- Add to the bottom of the file:
	- ```
		[DeepSeek]
		APIKey=
		Model=deepseek-chat
		Prompt=You are a professional translator. Translate the following text to English accurately. Maintain original context and nuance. Keep character names original.
		```
	- Add your own APIKey from [DeepSeek](https://platform.deepseek.com/) please note this is not free.
	- Change the translator section at the top of the file to use the translator:
	- ```
	  [Service]
	  Endpoint=DeepSeekTranslate
	  FallbackEndpoint=
	  ```

## Fine tuning your prompt

Please note the prompt is what actually tells DeepSeek what to translate. Some things that will help:
- Update the languages eg. Simplified Chinese to English, Japanese to English
- Ensure you add context to the prompt for the game such as 'Wuxia', 'Sengoku Jidai', 'Xanxia', 'Eroge'. 
- Make sure you tell it how to translate names whether you want literal translation or keep the original names

## Packages

The assemblies included are the Dev versions of XUnity.AutoTranslator.Feel free to replace them with the latest release if it doesnt work with your current build or do not trust the pre-built assemblies.