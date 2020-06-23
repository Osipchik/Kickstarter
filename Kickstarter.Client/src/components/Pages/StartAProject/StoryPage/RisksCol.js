import {LineInput} from "../../../Decorators/TextInput/TextFields";
import React from "react";
import {LearnMore} from "../LearnMore/LearnMore";
import PropTypes from 'prop-types';

const learMore = 'Communicate risks and challenges up front to set proper expectations. Learn more...';
const placeholder = 'Common risks and challenges you may want to address include budgeting, timelines for reward and the project itself< the size of you audience...';

export default function RisksCol(props) {
    const {className, onChange, text} = props;
    
    const onInputChange = newText => {
        onChange(newText);
    };
    
    return (
        <div className={className}>
            <LineInput
                value={text}
                multiline={true}
                onChange={(event) => onInputChange(event.target.value)}
                size="small"
                rowsMin="3"
                className="w-100"
                placeholder={placeholder}
                variant="outlined"
            />
            <LearnMore message={learMore}/>
        </div>
    )
}

RisksCol.propTypes = {
    className: PropTypes.string,
    onChange: PropTypes.func,
    text: PropTypes.string
};