# dictionary
<a href=""><img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white"></a>
<a href=""><img src="https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white"></a>
<br>
<b>Repetition is the key to learning.</b>

## What is dictionary?
`dictionary` is a CLI tool to help you get better at words.

## Table of contents
- [Usage](#usage)
- [File and name structure](#file-and-name-structure)
- [Configuration file](#configuration-file)
- [Files location](#files-location)
- [Statistics](#statistics)
- [Session Types](#session-types)
- [Modes](#modes)
- [Examples](#examples-of-commands)
    - [Edit your files](#edit)
    - [Normal session](#normal-session)
    - [Persistent mode](#persistent-mode)
    - [Learn and anwser mode](#learn-and-answer-mode)
    - [Status](#status)
- [How to install](#how-to-install)
- [Report a bug](#report-a-bug)

## Usage
Try the command `dictionary help` to get the full commands.

![help_menu.png](https://s10.gifyu.com/images/help_menu.png)

## File and name structure

#### File Structure
The file structure is simple, on the left you have your main word or words and 
on the right you have your meaning to that word and they are separated by a
pipe `|`:
```
word | meaning
```
If you want to put some synonyms you can do it by adding a comma `,`: 
```
word1, word2 | meaning
```
or 
```
word | meaning1, meaning2
```
you can put as many as you want for words as well for meanings:
```
word1, word2 | meaning1, meaning2
```
#### File Name Structure
The file name contains 3 parts:
- Name ( that can be whatever you want to describe what is in that file )
- Session Type ( see session types [here](#session-types) - this needs to be in all lower case )
- Extension ( This needs to be `txt` )

Example: `travel.words.txt` \
`travel` - Means there will be travel related words \
`words` - Session type - words \
`txt` - text extension

## Configuration file
The configuration file will help you have the best experience on your learning
and it looks like this:
```
{
    "OutputHasColors": true,
    "Over80IamCorrect": true,
    "AskMeSynonyms": true,
    "ReverseWords": true,
    "DisplayOneRandomSynonym": true, 
    "CurrentFile": "default.words",
    "DisplayFinalStatistics": true,
    "DisplayOnPairStatistics": true,
    "DisplayAvarageStatistics": true,
    "Layout": "card"
    "Mode": "persistent"
}
```
| Option                   | Description |
|--------------------------|-------------|
| OutputHasColors          | If you want to have colors in your output set this to true |
| Over80IamCorrect         | If you got an accuracy over 80% that means the answer is correct ( you can toggle this to remove typo mistakes )|
| AskMeSynonyms            | If you decide to put synonyms in your pairs, enable this to get asked those as well |
| ReverseWords             | `Only in words session type` <br> It will reverse the words with the meaning |
| DisplayOneRandomSynonym  | In the prompt you can be asked multiple words like <br> `What means -> word1 or word2` <br> Enable this to get a random one |
| CurrentFile              | When you run `dictionary start` this is the file that the session will start with, when you run `dictionary select` it will simply change this setting, and start the session with it |
| DisplayFinalStatistics   | At the end of every `session` it will be displayed statistics, toggle this as you like |
| DisplayOnPairStatistics  | At the end of every `question` it will be displayed statistics, toggle this as you like <br> This is helpful if you want to simulate a completly blind test |
| DisplayAvarageStatistics | `Only on irregular verbs session type` <br> it will display the avarage value from all 3 values |
| Layout                   | Setting it to `card` it will replace the question with the new one <br>or setting it to `list` it won't delete anything and let all the output there |
| Mode                     | It will set the current mode, see all the modes available [here](#modes), the mode must be written in lower case |

## Files location
| Files                 | Linux                              | Windows                                               |
|-----------------------|------------------------------------|-------------------------------------------------------|
| Words                 | `~/.local/share/dictionary/*`      | `C:\Users\user\AppData\Local\dictionary\data\*`       |
| Config                | `~/.config/dictionary/config.json` | `C:\Users\user\AppData\Local\dictionary\config\*`     |
| Statistics            | `~/.cache/dictionary/*`            | `C:\Users\user\AppData\Local\dictionary\statistics\*` |

## Statistics
| Type          | Description |
|---------------|-------------|
| Points        | Every time you answer correct to a question it will be added a point or points depending on your synonym settings and at the final you can see how many points you have accumulated |
| Response Time | `Only on CARD layout` <br> Display how long it took you to respond to a question |
| Accuracy      | With the help of `edit distance` algorithm and with some divisions you can see how well your answer matches in percentages % |

## Session Types
| Session         | Description |
|-----------------|-------------|
| Words           | You will be asked what means a word and you need to answer to what meaning you have put in your file |
| Irregular Verbs | For irregular verbs the file strucure needs to look like this <br> `word \| word1, word2, word3` <br> you can add synonyms on the left side as well. <br> It works like `words` type but with 3 questions |

## Modes
| Modes            | Description |
|------------------|-------------|
| Persistent       | You will have a normal session, but if you answer wrong to a question, it will be added to a list, after you finish the session, it will begin a new one with the wrong questions. This will be repeaded until you answer right to all the questions |
| Learn and Answer | When you start a session, you will be presented with 10 pairs from your file in order to 'learn them'. After that when you are ready it will start the session with these pairs. The process will continue until all your pairs are completed. You can see how many session you have up top |
| None             | This will change nothing to the behavior of the application |

## Examples of commands
### Edit
![](https://s10.gifyu.com/images/edit.gif)
### Normal session
![](https://s10.gifyu.com/images/none-mode.gif)
### Persistent mode
![](https://s10.gifyu.com/images/persistent-mode.gif)
### Learn and answer mode
![](https://s10.gifyu.com/images/learnandanswer-mode.gif)
### Status
This is a complete status information for 32 sessions

![](https://s10.gifyu.com/images/status_command.png)

## How to install
The application supports only 2 platforms `Linux` and `Windows` <br>
If you want to install it, you can run:
- On Linux: `dotnet publish -r linux-x64 --self-contained true --output <location>`
- On Windows: `dotnet publish -r win-x64 --self-contained true --output <location>`

## Report a bug
> If you want to report a bug, please right an issue that will contain the followings: <br> 
- Your configuration file settings
- The pair on that the bug occured.

