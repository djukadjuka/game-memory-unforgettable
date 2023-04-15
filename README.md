# The *Unforgettable* Memory Game
A memory game so artistic that it's simply unforgettable. 
I have tried using the same-ish art style on all cards so that the cards are similar, thus making it a bit harder to remember which is which. 
Points are based on the time it took to find a pair. Each pair brings 10 points, multiplied by the time factor, where the time factor TF goes from 1 under 45 seconds, to 0.5 over 120 seconds [with additional steps between].


## The Spec

Basic Mechanics

 - [ ] Open two cards per turn
 - [ ] Opening same two cards - Cards stay open - Add Points to player
 - [ ] Opening different two cards - Cards close after delay
 - [ ] Opening any two cards - Increase number of turns
 - [ ] Implement elapsed time since game started
 - [ ] Button for restarting game
 - [ ] Shuffle cards and their positions on game start/restart
 - [ ] Background music
 - [ ] Sound effect after opening any one card
 - [ ] Build game for mobile and for PC
 - [ ] Put project on git and have normal commit messages
 - [ ] End of game is when player opens all cards
 - [ ] At end of game, congratulate player based on number of points
 - [ ] Number of points is multiplied by elapsed time factor [eg, seconds under 60 is 1 \* points, seconds between 60 and 120 is 0.9 \* points..]
