import React, {Component} from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor() {
        super();
        this.state = {
            nickname: ""
        }
    }

    async componentDidMount() {
        let response = await fetch("users/me");
        const data = await response.json();
        this.setState({nickname: data.nickname})
    }

    render() {
        return (
            <div>
                <h4>{this.state.nickname} 님, 안녕하세요!</h4>
                <a href={"https://localhost:8080/auth/logout"}>Logout</a>
            </div>
        );
    }
}
