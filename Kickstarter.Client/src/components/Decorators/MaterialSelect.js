import React from "react";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import {LineInput} from "./TextInput/TextFields";
import PropTypes from "prop-types";

export default function MaterialSelect(props) {    
    const placeholder = props.placeholder !== undefined ? <MenuItem value={0} disabled>{props.placeholder}</MenuItem> : null;

    let propertyNames = null;
    const fillMenu = () => {
        if(props.items === undefined || !props.items.length){
            return;
        }
        
        propertyNames = Object.getOwnPropertyNames(props.items[0]);
        let i = 1;
        for(i; i < propertyNames.length; i++){
            if(!propertyNames[i].includes('Id')){
                break;
            }
        }

        return(
            props.items.map(item =>
                <MenuItem key={getKey(item, i)} value={item[propertyNames[0]]}>{item[propertyNames[i]]}</MenuItem>
            )
        )
    };
    
    const getKey = (item, count) => {
        let key = `${item[propertyNames[0]]}`;
        for(let i = 1; i < count; i++){
            key += `-${item[propertyNames[i]]}`
        }
        
        return key;
    };
    
    return(
        <Select
            className={props.className}
            value={props.value}
            onChange={(event) => props.onChange(event.target.value)}
            input={<LineInput />}>
            {placeholder}
            {fillMenu()}
        </Select>
    )
}

MaterialSelect.propTypes = {
    className: PropTypes.string,
    value: PropTypes.number,
    onChange: PropTypes.func,
    placeholder: PropTypes.string,
    items: PropTypes.array
};