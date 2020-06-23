import Typography from "@material-ui/core/Typography";
import {Select} from "antd";
import Option from "rc-mentions/lib/Option";
import React from "react";
import PropTypes from 'prop-types';

export default function Sort(props) {
    return(
        <div className="col mr-3">
            <div className="row float-right">
                <Typography
                    variant="subtitle1"
                    gutterBottom
                    className="mt-1 mx-1" >
                    Sort by
                </Typography>
                <Select
                    defaultValue="jack"
                    onChange={props.onSelect}
                    className="select">
                    <Option value="jack">
                        jack
                    </Option>
                    <Option value="lucy">Lucy</Option>
                    <Option value="Yiminghe">yiminghe</Option>
                </Select>
            </div>
        </div>
    )
}

Sort.propTypes = {
    onSelect: PropTypes.func
};