October 6th, 2020
We don't know how to do much at all with Forms, or the GUI. 
Getting together the GitHub code, and fixing our code after pulling it from GitHub was a process, as well as adding the GUI, and PS6 Skeleton. 
We decided to make the Cell Contents box the biggest, out of the 3 text boxes, so you could have more than enough room to write long formulas. 
The Value box wouldn't need to be as big, because the formula's should be evaluated before input goes into the value box. 

October 7th, 2020
Added implementation for changing cells, and changing contents of cell.
Lots of changes in spreadsheet controller, to actually be able to control the spreadsheet.
We figured out more about how Form works, and implemented a lot of events. 

October 8th, 2020
Implemented all of the file things (ie Save file, Quit, New file, and Open file)
Highlighting cells now works properly
Added the making sure you want to exit message, even if the sheet hasn't been saved. 
Starting Cell Name now starts with A1

October 9th, 2020
We made our "special feature". Its where you can use the Up Arrow key to create a sum of all cells above and in the same column as the current selection,
and use The Left Arrow key to create a sum of all cells to the left and in the same row as te current selection.
The Sums will ignore all cells containing text
Because our spreadsheet implementation would come up with formula errors if you =A1, but A1 doesn't have any contents, we can't make the sum feature sum
the empty cells.