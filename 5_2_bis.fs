: read-number ( addr1 u1 -- d addr2 u2 )
    begin over C@ 32 = while 1- swap 1+ swap repeat
    0 0 2swap >number ;

: min-array ( addr u -- n )
    cells over + swap
    dup @ rot rot do
        I @ 2dup > if nip else drop then
    cell +loop
;

variable nbseeds 20 nbseeds !
create seeds 1000 cells allot
\ create seed-soil 200 cells allot
\ create soil-fertilizer 200 cells allot
\ create fertilizer-water 200 cells allot
\ create water-light 200 cells allot
\ create light-temperature 200 cells allot
\ create temperature-humidity 200 cells allot
\ create humidity-location 200 cells allot

create maps 2000 cells allot

: parse ( addr u -- )
    6 - swap 6 + swap
    0 rot rot
    \ rot 6 + rot 6 - rot
    begin over C@ 10 <> while
        read-number 2swap drop
        3 pick cells seeds + !
        rot 1+ rot rot
        read-number 2swap drop
        3 pick 1- cells seeds + @ + 3 pick cells seeds + !
        rot 1+ rot rot
    repeat
    rot drop 0 rot 2 + rot 2 -
    begin dup 0> while
        begin over C@ 10 <> while 1- swap 1+ swap repeat 1- swap 1+ swap \ skip everything until next line
        \ 2dup read-number 2swap 2drop drop 2 pick = if 
        dup 0<= if 1 else over C@ 10 = then if 
            \ end of map
            rot dup cells maps + -1 swap ! 1+ rot 1+ rot 1-
        else
            \ 2dup drop 20 type CR
            read-number 2swap drop 3 pick cells maps + ! rot 1+ rot rot
            read-number 2swap drop 3 pick cells maps + ! rot 1+ rot rot
            read-number 2swap drop 3 pick 1- cells maps + @ + 3 pick cells maps + ! rot 1+ rot rot
        then
        \ 1- swap 1+ swap \ skip \n
        \ test end of map
    repeat
    2drop
;

: 3dup { a b c } a b c a b c ;

: process-interval { sfrom sto iseed mend mpos }
    begin mpos mend < while
        mpos @
        dup -1 <> if
            mpos cell+ @ mpos 2 cells + @
            .s CR
            over sfrom > if
                \ début seed < début intervalle
                over sto >= if
                    \ fin seed <= début intervalle
                    \ intervalle:          |-----------|
                    \ seed:       |-------|
                    ." CASE 1" CR
                    2drop drop mpos 3 cells + to mpos
                else
                    \ fin seed > début intervalle
                    dup sto >= if
                        \ fin seed <= fin intervalle
                        \ intervalle:      |-----------|
                        \ seed:       |---------|
                        ." CASE 2" CR
                        over seeds nbseeds @ cells + !
                        sto seeds nbseeds @ 1+ cells + !
                        over seeds iseed 1+ cells + !
                        nbseeds @ dup 2 + nbseeds ! nip
                        over sto rot mend mpos .s CR recurse
                        nip to sto mpos 3 cells + to mpos
                    else
                        \ fin seed > fin intervalle
                        \ intervalle:      |-----------|
                        \ seed:       |--------------------|
                        ." CASE 3" CR
                        over seeds nbseeds @ cells + !
                        dup seeds nbseeds @ 1+ cells + !
                        dup seeds nbseeds @ 2 + cells + !
                        sto seeds nbseeds @ 3 + cells + !
                        over seeds iseed 1+ cells + !
                        nbseeds @ dup 4 + nbseeds !
                        3dup mend mpos recurse
                        sto swap 2 + mend mpos recurse
                        drop nip to sto mpos 3 cells + to mpos
                    then
                then
            else
                \ début seed >= début intervalle
                dup sfrom <= if
                    \ début seed >= fin intervalle
                    \ intervalle:  |-----------|
                    \ seed:                     |---------|
                    ." CASE 4" CR
                    2drop drop mpos 3 cells + to mpos
                else
                    \ début seed < fin intervalle
                    dup sto >= if
                        \ fin seed <= fin intervalle
                        \ intervalle:      |-----------|
                        \ seed:               |----|
                        ." CASE 5" CR
                        drop - dup sto + to sto sfrom + to sfrom
                        sfrom seeds iseed cells + !
                        sto seeds iseed 1+ cells + !
                        begin mpos @ -1 <> while mpos cell+ to mpos repeat mpos cell+ to mpos
                    else
                        \ fin seed > fin intervalle
                        \ intervalle:      |-----------|
                        \ seed:                  |-----------|
                        ." CASE 6" CR
                        dup seeds nbseeds @ cells + !
                        sto seeds nbseeds @ 1+ cells + !
                        dup seeds iseed 1+ cells + !
                        nbseeds @ dup 2 + nbseeds !
                        ." ------------------------------------------------------------------------------- "
                        over sto rot mend mpos .s CR recurse
                        ." -> " .s CR
                        to sto - dup sto + to sto sfrom + to sfrom
                        sfrom seeds iseed cells + !
                        sto seeds iseed 1+ cells + !
                        begin mpos @ -1 <> while mpos cell+ to mpos repeat mpos cell+ to mpos
                    then
                then
            then
        else
            drop mpos cell+ to mpos
        then
        .s CR
    repeat
;

: advcode
    parse CR
    3 0 do
        seeds I cells + @
        seeds I 1+ cells + @
        I
        3 pick cells maps +
        maps
        .s CR
        process-interval
        \ bye
    loop 
    
    CR
    nbseeds @ cells seeds + seeds do
        I @ . ." "
    cell +loop
    
    CR
    
    seeds nbseeds @ min-array
;

create buffer 1000000 allot

: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input5" open-read buffer swap advcode CR . CR bye