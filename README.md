# WordleBestWord

The game of wordle consists of a hidden word that the user must guess. Solutions and guesses must be the same length.
Each guess is scored using a series of colored blocks, one for each letter. For each position, if the
letter in the solution matches the guess, a green block is displayed. If the letter in the guess is
contained in the solution, but not in the current position a yellow block is displayed. If the letter
in the guess is not contained in the solution, the block remains grey.

The first guess is effectivly random because the user is not given information about the game's state
until the first guess is scored. 

To efficiently solve the puzzle, each guess should match the results of each previous guess. If the number of
possible guess words is fixed, then each guess and score reducess the number of valid guesses from the set
of all guesses.

This analysis uses a simplified version of the wordle scoring mechanism to calculate the number of guess
words that are valid after the opening guess for a given solution and guess word. This analysis is repeated
for each guess for each solution. The scores are then summarized across all solution words for
each guess word. The word with the lowest score can be considered the best opening word

To speed up the search for the best opening word, this analysis only considers candidate words
that are also possible solitions and are composed of unique letters. To make the summary more robust
the interquartile mean is used instead of the average.
