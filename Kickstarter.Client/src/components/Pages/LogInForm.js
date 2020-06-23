import React from 'react'
import {RedditTextField} from "../Decorators/TextInput/TextFields"
import {Password} from "../Decorators/TextInput/Password"
import PropTypes from "prop-types"
import '../../styles/UserAccountForm.css'

export const LogInForm = (props) => {
    const {onEmailChange, onPasswordChange, emailInvalid, passwordInvalid} = props

    return(
        <div>
            <div className="pb-2">
                <RedditTextField
                    label="E-mail"
                    className="w-100"
                    onChange={event => onEmailChange(event.target.value)}
                />
                <p className="mt-1 text-danger">{emailInvalid.message}</p>
            </div>
            <div className="pb-2">
                <Password
                    label="Password"
                    className="w-100"
                    onChange={event => onPasswordChange(event.target.value)}
                />
                <p className="mt-1 text-danger">{passwordInvalid.message}</p>
            </div>
        </div>
    )
}

LogInForm.propTypes = {
    onEmailChange: PropTypes.func,
    onPasswordChange: PropTypes.func,
    emailInvalid: PropTypes.object,
    passwordInvalid: PropTypes.object
}