import React from "react";
import CircularProgress from "@material-ui/core/CircularProgress";
import { Progress } from 'reactstrap';

export function Loading() {
    return(
        <div className="d-flex justify-content-center">
            <CircularProgress className="text-center"/>
        </div>
    )
}

export function ProgressBar(props) {
    return(
        <Progress value={props.value} />
        // <Progress value={props.value} className="progress-bar"/>
        // <LinearProgress
        //     color="primary"
        //     className={classes.root}
        //     variant="determinate"
        //     value={props.value > 100 ? 100 : props.value}/>
    )
}