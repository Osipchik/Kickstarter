import React, { useState } from "react";
import {LineInput} from "../../../Decorators/TextInput/TextFields";
import PropTypes from "prop-types";
import {ComponentTitle} from "../ComponentTitle";

export default function GoalCol(props) {
    const {onGoalChange, className} = props;
    const [goal, setGoal] = useState(props.goal);

    const onChange = goal => {
        let num = Number(goal);

        if (!isNaN(num) && num >= 0){
            onGoalChange(num)
            setGoal(num)
        }
    };

    const onClick = () => {
        if (goal === 0) {
            setGoal('')
        }
    }

    const onFocusout = () => {
        if (goal === '') {
            setGoal(0)
        }
    }

    return(
        <div className={className}>
            <ComponentTitle title={'Goal amount'}/>
            <div className="input-group">
                <div className="input-group-prepend">
                    <span className="input-group-text" id="basic-addon1">AU$</span>
                </div>
                <LineInput
                    size="small"
                    value={goal}
                    onChange={(event) => onChange(event.target.value)}
                    onClick={() => onClick()}
                    onBlur={() => onFocusout()}
                    className="col p-0 m-0"
                    variant="outlined"/>
            </div>
        </div>
    )
}

GoalCol.propTypes = {
    onGoalChange: PropTypes.func,
    className: PropTypes.string
};