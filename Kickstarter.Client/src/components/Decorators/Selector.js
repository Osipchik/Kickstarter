import React from 'react';
import PropTypes from 'prop-types';
import { Select } from 'antd';
import Typography from "@material-ui/core/Typography";
import Box from "@material-ui/core/Box";
const { Option } = Select;

export default function Selector(props) {
    const { onSelect, collection, placeholder, className, defaultValue} = props;
    
    return (
        <div className="py-2">
            {defaultValue != null 
                ?  <Select
                    className={className}
                    placeholder={placeholder}
                    optionFilterProp="children"
                    onSelect={onSelect}
                    defaultValue={defaultValue}
                    size="large">
                    {collection.map(item =>
                        <Option
                            key={item.id}
                            value={item.id}
                            className="text-dark">
                            <Typography component="div">
                                <Box fontWeight="fontWeightRegular" m={1}>
                                    {item.categoryName}
                                </Box>
                            </Typography>
                        </Option>
                    )}
                </Select>
                :
                <Select
                    className={className}
                    placeholder={placeholder}
                    optionFilterProp="children"
                    onSelect={onSelect}
                    size="large">
                    {collection.map(item =>
                        <Option
                            key={item.id}
                            value={item.id}
                            className="text-dark">
                            <Typography component="div">
                                <Box fontWeight="fontWeightRegular" m={1}>
                                    {item.categoryName}
                                </Box>
                            </Typography>
                        </Option>
                    )}
                </Select>}
        </div>
    );
}

Selector.propTypes = {
    onSelect: PropTypes.func,
    collection: PropTypes.array,
    placeholder: PropTypes.string,
    className: PropTypes.string,
    defaultValue: PropTypes.number
};