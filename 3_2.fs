variable len
variable sum

: is-number ( addr -- b ) C@ dup 48 >= swap 57 <= and ;

: get-number-begin-addr ( addr1 -- addr2 )
  begin dup is-number while 1- repeat 1+
;

: check-gear ( addr1 -- addr2 addr3 b ) { addr }
  0
  addr len @ - C@ '.' = if
    addr 1- len @ - dup is-number if get-number-begin-addr else drop then
    addr 1+ len @ - dup is-number if get-number-begin-addr else drop then
  else
    addr len @ - dup is-number if get-number-begin-addr else drop then
  then

  addr len @ + C@ '.' = if
    addr 1- len @ + dup is-number if get-number-begin-addr else drop then
    addr 1+ len @ + dup is-number if get-number-begin-addr else drop then
  else
    addr len @ + dup is-number if get-number-begin-addr else drop then
  then

  addr 1- dup is-number if get-number-begin-addr else drop then
  addr 1+ dup is-number if get-number-begin-addr else drop then

  depth 3 >= if
    2 pick if if if then then 0
    else rot drop 1 then
  else depth 0 do drop loop 0 then
;

: parse ( addr1 u1 -- )
  S" *" 2swap
  begin 2over search while
    over check-gear
    if 10 0 0 2swap >number 2drop drop dup . swap ." * "
       10 0 0 2swap >number 2drop drop dup . ." = "
       * dup . CR sum @ + sum !
    then
    1- swap 1+ swap
  repeat
;

create buffer 1000000 allot

: get-line-length ( x name -- x n )
  R/O open-file drop dup dup file-size drop drop buffer swap rot read-line 2drop swap close-file drop ; 

: fill ( buf c len -- )
  0 do dup 2 pick I + ! loop 2drop ;

: open-read ( x name u buf -- x n ) { name u buf }
  name u R/O open-file drop dup dup file-size drop drop buf swap rot read-file drop swap close-file drop
;

: advcode 0 sum ! 
  2dup get-line-length len ! \ get line length
  buffer '.' len @ fill \ fill a first line with dots
  len @ buffer + dup 10 swap ! 1+ \ add \n and shift pointer to buffer
  rot rot 2 pick open-read \ read the file
  swap over + '.' len @ fill \ fill last line with dots
  len @ 1+ len ! \ increment len because of \n at end of lines
  CR buffer swap len @ 2* +
  0 sum ! parse sum @
;

0 S" input3" advcode . CR bye