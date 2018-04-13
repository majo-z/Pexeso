# Pexeso Game
Pexeso, also known as Matching Pairs or Concentration game, is a card game in which all of the cards are laid face down on a board and only two cards are flipped face up over each turn. The player tries to remember the subject of the pictures and their position. The object of the game is to search out and collect all identical picture pairs. 

## Prerequisites
The application has be written in C# programming language and created for the Universal Windows Platform (UWP). It is compatible and can be deployed on any device running Windows 10. 

## Functionality of the app

The application has been created using pivot control and allows for navigation between three content panes - Game, Grid Size and Game History.

Grid Size section is an initial pane when the application starts and allows the user to choose a grid size, either 4x4, 6x6 or 8x8. A grid a generated at run time and populated with pairs of images which are linked together.

The grid size determines the amount of time the user gets to memorize the cards before they are flipped over. The larger the grid size, the more time given.

When the user chooses the grid size, it is redirected to main Game section page. The user is able to play the game by identifying matching pairs. Once a pair has been found they will remain face up and this continues until all cards are face up. When all pairs have been found, the "Game Over!" message will be displayed in botom left corner. Once the game is over, the details are recorded and can be seen in Game History section. The user can play another game by clicking on New Game button or navigate to Grid Size section to choose from a different grid size. 

### Use of Data Binding and Local Storage

The device local storage is used to keep track of game scores (high score and game score), grid sizes and the dates the games were played.

A Game object is used to represent past games the user has played. Through the use of data binding, these objects can be visualised on Game History page.

## How to download and run the program

The user can download the program directly from Microsoft Application Store and install it on the device. The link to the store application can be found [here](https://www.microsoft.com/store/apps/9P5B0X5PTRJ8).

Alternatively, the user can download the application from github repository, that can be found at https://github.com/majo-z/Pexeso. The application has been build using Microsoft Visual Studo 2017 and to run the code in this repository, the files must first be compiled. The [Microsoft Visual Studo 2017](https://www.visualstudio.com/downloads/) and [Git](https://git-scm.com/) must first be installed on the computer. Once they are installed, the code can be compiled and run by following steps below. It is assumed the command line is used for cloning the repository.

1. Open the Git Bash and clone the repository 
```bash
> git clone https://github.com/majo-z/Pexeso
```
2. Navigate to cloned repository 
```bash
> double click on Pexeso.sln file 
```
3. Alternatively, open Microsoft Visual Studio 2017
```bash
> click on File -> Open -> Project/Solution -> Navigate to cloned repository folder -> Pexeso.sln
```
4. The application has to be rebuild first
```bash
> click Build -> Rebuild Solution
```
5. Make sure Debug is displayed in Solution Configuration and x86 is displayed in Solution Platform window
```bash
> click on Local Machine button
```
6. The application will be compiled and installed on the device

## Learn more about Universal Windows Platform
https://docs.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide

https://docs.microsoft.com/en-us/windows/uwp/get-started/create-a-hello-world-app-xaml-universal