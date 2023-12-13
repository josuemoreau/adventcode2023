: insertion-sort { t n step }
    t n step * + t do
        I step -
        begin dup t >= if dup @ over step + @ 2dup > if 1 else 2drop drop 0 then else drop 0 then while
            2 pick ! over step + ! step -
        repeat
    step +loop
;