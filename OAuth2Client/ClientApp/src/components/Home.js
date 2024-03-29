import React, {Component} from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
            nickname: "",
            isAuthenticated: false,
            isAuthorized: false,
            color: null
        }

        fetch("users/me")
            .then(async res => {
                switch (res.status) {
                    case 200:
                        this.state.isAuthenticated = true;
                        const data = await res.json();
                        this.setState({nickname: data.nickname})
                        break;

                    case 401:
                        this.state.isAuthenticated = false;
                        break;
                }
            });
        
        fetch("resources")
            .then(async res => {
                const data = await res.json();
                this.setState({color: data.color})
            })
    }

    render() {
        return (
            <div style={{textAlign: "center", paddingTop: "50px"}}>
                <h4 style={{color: this.state.color}}>
                    {this.state.isAuthenticated ? this.state.nickname + " 님, 안녕하세요!?" : "로그인이 필요합니다."}
                </h4>
                <a
                    href={
                        this.state.isAuthenticated
                            ? "https://localhost:8080/auth/logout"
                            : "https://localhost:8080/auth/challenge"
                    }
                >
                    {this.state.isAuthenticated ? "로그아웃" : "카카오 로그인"}
                </a>
            </div>
        );
    }
}
