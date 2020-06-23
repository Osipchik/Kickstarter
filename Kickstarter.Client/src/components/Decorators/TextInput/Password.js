import React from 'react';
import InputAdornment from "@material-ui/core/InputAdornment";
import IconButton from "@material-ui/core/IconButton";
import Visibility from "@material-ui/icons/Visibility";
import VisibilityOff from "@material-ui/icons/VisibilityOff";
import {useStylesReddit} from "./InputStyles";
import TextField from "@material-ui/core/TextField";

export function Password(props) {
    const [values, setValues] = React.useState({
        showPassword: false,
        password: '',
    });

    const handleClickShowPassword = () => {
        setValues({ ...values, showPassword: !values.showPassword });
    };

    const handleChange = prop => event => {
        setValues({ ...values, [prop]: event.target.value });
    };

    const handleMouseDownPassword = event => {
        event.preventDefault();
    };

    const classes = useStylesReddit();

    return (
        <TextField
            onChange={handleChange('password')}
            type={values.showPassword ? 'text' : 'password'}
            InputProps={{ classes, 
                disableUnderline: true,
                endAdornment: (
                    <InputAdornment position="end">
                        <IconButton
                            aria-label="toggle password visibility"
                            onClick={handleClickShowPassword}
                            onMouseDown={handleMouseDownPassword}
                            edge="end">
                            {values.showPassword ? <Visibility /> : <VisibilityOff />}
                        </IconButton>
                    </InputAdornment>
                ),}} 
            {...props} 
            variant="filled"
        />
    );
}