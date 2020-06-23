import React, { useState } from "react";
import { DateTimePicker , MuiPickersUtilsProvider } from "@material-ui/pickers";
import MomentUtils from '@date-io/moment';
import moment from "moment";
import "moment/locale/ru";
import { string } from "prop-types";

function DateFnsLocalizationExample(props) {
    const {value, onChange} = props;
    const [selectedDate, setDate] = useState(value === null ? moment() : value);

    if (selectedDate instanceof string) {
        setDate(moment.utc(selectedDate).local().format());
    }

    const handleDateChange = (date) => {
        setDate(date)
        onChange(date)
    }


    return (
        <MuiPickersUtilsProvider libInstance={moment} utils={MomentUtils}>
            <DateTimePicker
                autoOk={true}
                ampm={false} 
                value={selectedDate}
                minDate={moment()}
                maxDate={moment(moment()).add(2, 'M')}
                onChange={date => handleDateChange(date)}
            />
        </MuiPickersUtilsProvider>
    );
}

export default DateFnsLocalizationExample;