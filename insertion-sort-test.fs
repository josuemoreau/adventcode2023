require random.fs
require insertion-sort.fs

variable n
10 n !
create buffer n @ cells allot

1867328 seed !

: test
    n @ 0 do
        I 1+ buffer I cells + !
    loop ;

: print
    n @ dup 0 do
        buffer I cells + @ . CR
    loop drop ;

: swap-cells ( addr1 addr2 -- )
    dup @ 2 pick @ rot ! swap !
;

: shuffle { t n }
    n 0 do
        n random cells t + t I cells + swap-cells
    loop ;

test CR
print ." ----" CR
buffer n @ shuffle
print ." ----" CR
buffer n @ 1 cells insertion-sort print bye