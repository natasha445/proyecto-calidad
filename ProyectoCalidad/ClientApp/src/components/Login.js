import React, { Component } from 'react';
import './Login.css';
import axios from 'axios';
import MessageModal from './MessageModal.js';
var CryptoJS = require("crypto-js");

export class Login extends Component {
    static displayName = Login.name;

    constructor(props) {
        super(props);

        this.validateCredentials = this.validateCredentials.bind(this);
        this.onUserInputChange = this.onUserInputChange.bind(this);
        this.onPasswordInputChange = this.onPasswordInputChange.bind(this);
        this.hideModal = this.hideModal.bind(this);

        this.state = {
            validCredentials: false,
            userName: "",
            password: "",
            show: false,
            errorMessage: "",
            loginAttempts: 0
        };
    }

    validateCredentials() {
        if (this.state.userName === "" || this.state.password === "") {
            this.setState({
                show: true,
                errorMessage: "Username or password cannot be empty"
            });
        } else {
            axios.post('User/ValidateCredentials', {
                UserName: this.state.userName
            }).then((success) => {
                if (success.data[0]) {
                    if (success.data[0].isLocked && success.data[0].unlockDate) {
                        const currentDate = new Date();
                        const unlockDate = new Date(success.data[0].unlockDate);

                        if (unlockDate >= currentDate) {
                            this.setState({
                                show: true,
                                errorMessage: "User is locked, try in a few minutes"
                            });
                        } else {
                            axios.post('User/UnlockUser', {
                                UserName: this.state.userName
                            });

                            this.setState({
                                show: true,
                                errorMessage: "User is now unlocked, try login again",
                                isSuccess: true
                            });
                        }
                    } else {
                        const decryptedData = CryptoJS.AES.decrypt(success.data[0].password, '/B?E(H+MbQeShVmY');
                        const decryptedPassword = decryptedData.toString(CryptoJS.enc.Utf8);

                        if (this.state.password === decryptedPassword) {
                            this.props.history.push({
                                pathname: '/Agenda',
                                state: {
                                    userName: this.state.userName
                                }
                            });
                        } else {
                            axios.post('User/FailedLogin', {
                                UserName: this.state.userName
                            });

                            this.setState({ loginAttempts: this.state.loginAttempts + 1 });

                            if (this.state.loginAttempts >= 3) {
                                const date = new Date();
                                const dateToMilliseconds = date.getTime();
                                const addedMinutes = dateToMilliseconds + (60000 * 3);
                                const unlockDate = new Date(addedMinutes).toString();

                                axios.post('User/LockUser', {
                                    UserName: this.state.userName,
                                    LockUser: '1',
                                    UnlockDate: unlockDate
                                });
                            }

                            this.setState({
                                validCredentials: false,
                                show: true,
                                errorMessage: "Credentials are invalid"
                            });
                        }
                    }
                } else {
                    this.setState({
                        show: true,
                        errorMessage: "Credentials are invalid"
                    });
                }
            }).catch((error) => {
                this.setState({
                    show: true,
                    errorMessage: "Server could not be contacted, try in some minutes"
                });
            });
        }
    }

    hideModal() {
        this.setState({ show: false });
    };

    onUserInputChange(event) {
        this.setState({
            userName: event.target.value
        });
    }

    onPasswordInputChange(event) {
        this.setState({
            password: event.target.value
        });
    }

    render() {
        return (
            <section id="login" className="login">
                <div className="title">
                    <h1>Login</h1>
                </div>
                <div className="form">
                    <input
                        type="text"
                        placeholder="Username"
                        className="text"
                        name="username"
                        value={this.state.userName}
                        onChange={this.onUserInputChange}
                        required>
                    </input>
                    <br></br>
                    <input
                        type="password"
                        placeholder="Password"
                        className="password"
                        name="password"
                        value={this.state.password}
                        onChange={this.onPasswordInputChange}
                        required>
                    </input>
                    <br></br>
                    <button
                        type="submit"
                        id="login-button"
                        className="login-button"
                        onClick={this.validateCredentials}>
                        Login
                    </button>
                    <a href="/SignUp" className="forgot-password">
                        Register
                    </a>
                </div>
                {this.state.show &&
                    <MessageModal handleClose={this.hideModal} isSuccess={this.state.isSuccess}>
                        <p className="modal-text">
                            {this.state.errorMessage}
                        </p>
                    </MessageModal>
                }
            </section>
        );
    }
}
