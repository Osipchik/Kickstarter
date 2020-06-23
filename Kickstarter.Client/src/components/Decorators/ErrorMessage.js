import React from 'react'
import {Typography} from "@material-ui/core";
import Box from "@material-ui/core/Box";
import PropTypes from "prop-types"

export const ErrorMessage = (props) => {
    return(
        <Typography component="div" className="mt-1 text-danger">
            <Box
                className="mr-auto"
                fontSize="body2.fontSize"
                fontWeight="fontWeightLight">
                {props.message}
            </Box>
        </Typography>
    )
}

ErrorMessage.propTypes = {
    message: PropTypes.string
}