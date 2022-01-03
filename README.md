# dictionary
<a href=""><img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white"></a>
<a href=""><img src="https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white"></a>
<br>
Repetition is the key to learning.

## What is dictionary?
`dictionary` is a CLI tool to help you get better at words.

## Table of contents
- [Usage](#usage)
- [File and name structure](#file-and-name-structure)
- [Configuration file](#configuration-file)
- [Statistics](#statistics)
- [Session Types](#session-types)
- [How to install](#how-to-install)
- [Report a bug](#report-a-bug)

## Usage
Try the command `dictionary help` to get the full commands.
```
Usage: dictionary <options>

Options:
start - Starts a session with the default configuration.
edit  - Opens an editor to edit either your config or words file.
             Example: 'dictionary edit config'.
select - Select the current words file.
status - Display status information about your sessions.
help   - Displays this help menu.
```

## File and name structure

#### File Structure
The file structure is simple, on the left you have your main word or words and 
on the right you have your translation to that word and they are separated by a
pipe `|`:
```
word | translation
```
If you want to put some synonyms you can do it by adding a comma `,`: 
```
word1, word2 | translation
```
or 
```
word | translation1, translation2
```
you can put as many as you want for words as well for translations:
```
word1, word2 | translation1, translation2
```
#### File Name Structure
The file name contains 3 parts:
- File name ( that can be whatever you want to describe what is in that file )
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
    "DisplayOneRandomSynonym": true, 
    "CurrentFile": "default.words",
    "DisplayFinalStatistics": true,
    "DisplayOnPairStatistics": true,
    "DisplayAvarageStatistics": true,
    "Layout": "card"
}
```
| Option                   | Description |
|--------------------------|-------------|
| OutputHasColors          | If you want to have colors in your output set this to true |
| Over80IamCorrect         | If you got an accuracy over 80% that means the answer is correct ( remove typo mistakes )|
| AskMeSynonyms            | If you decide to put synonyms in your pairs, enable this to get asked those as well |
| DisplayOneRandomSynonym  | In the prompt you can be asked multiple words like <br> `What means -> word1 or word2` <br> Enable this to get a random one |
| CurrentFile              | When you run `dictionary start` this is the file that the session will start with |
| DisplayFinalStatistics   | At the end of every `session` it will be displayed statistics, toggle this as you like |
| DisplayOnPairStatistics  | At the end of every `question` it will be displayed statistics, toggle this as you like <br> This is helpful if you want to simulate a completly blind test |
| DisplayAvarageStatistics | `Only on irregular verbs session type` <br> it will display the avarage value from all 3 values |
| Layout                   | Setting it to `card` it will replace the question with the new one <br>or setting it to `list` it won't delete anything and let all the output there |

## Statistics
| Type          | Description |
|---------------|-------------|
| Points        | Every time you answer correct to a question it will be added a point and at the final you can see how many points you have accumulated |
| Response Time | `Only on CARD layout` <br> Display how long it took you to respond to a question |
| Accuracy      | With the help of `edit distance` algorithm and with some divisions you can see how well your answer matches in percentages % |

## Session Types
| Session         | Description |
|-----------------|-------------|
| Words           | You will be asked what means a word and you need to answer to what translation you put in your file |
| Irregular Verbs | For irregular verbs the file strucure needs to look like this <br> `word \| word1, word2, word3` <br> you can add synonyms on the left side as well. <br> It works like `words` type but with 3 questions |

## How to install
The application supports only 2 platforms `Linux` and `Windows` <br>
If you want to install it, you can run:
- On Linux: `dotnet publish -r linux-x64 --self-contained true --output <location>`
- On Windows: `dotnet publish -r win-x64 --self-contained true --output <location>`

## Report a bug
> If you want to report a bug, please right an issue that will contain the followings: <br> 
- Your configuration file settings
- The pair on that the bug occured.

