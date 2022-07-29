import React, { Component } from 'react';
import axios from 'axios';
import moment from 'moment';
import MessageModal from './MessageModal.js';
import { ReactAgenda, ReactAgendaCtrl, guid, getUnique, getLast, getFirst, Modal } from 'react-agenda';
import './Agenda.css';

var now = new Date();

require('moment/locale/en-gb');
var colors = {
    'color-1': "rgba(102, 195, 131 , 1)",
    "color-2": "rgba(242, 177, 52, 1)",
    "color-3": "rgba(235, 85, 59, 1)",
    "color-4": "rgba(70, 159, 213, 1)",
    "color-5": "rgba(170, 59, 123, 1)"
}

export default class Agenda extends Component {
    constructor(props) {
        super(props);

        this.state = {
            items: [],
            selected: [],
            cellHeight: (60 / 4),
            showModal: false,
            locale: "en-gb",
            rowsPerHour: 4,
            numberOfDays: 4,
            startDate: new Date(),
            message: "",
            show: false,
            isSuccess: false
        }

        this.handleRangeSelection = this.handleRangeSelection.bind(this);
        this.handleItemEdit = this.handleItemEdit.bind(this);
        this.zoomIn = this.zoomIn.bind(this);
        this.zoomOut = this.zoomOut.bind(this);
        this._openModal = this._openModal.bind(this);
        this._closeModal = this._closeModal.bind(this);
        this.addNewEvent = this.addNewEvent.bind(this);
        this.removeEvent = this.removeEvent.bind(this);
        this.editEvent = this.editEvent.bind(this);
        this.changeView = this.changeView.bind(this);
        this.handleCellSelection = this.handleCellSelection.bind(this);
        this.hideModal = this.hideModal.bind(this);
        this.getEvents = this.getEvents.bind(this);
        this.updateEvent = this.updateEvent.bind(this);
    }

    async componentDidMount() {
        this.getEvents();
    }

    componentWillReceiveProps(next, last) {
        if (next.items) {
            this.setState({ items: next.items });
        }
    }

    handleItemEdit(item, openModal) {
        if (item && openModal === true) {
            this.setState({ selected: [item] });
            return this._openModal();
        }
    }

    handleCellSelection(item, openModal) {
        if (this.state.selected && this.state.selected[0] === item) {
            return this._openModal();
        }

        this.setState({ selected: [item] });
    }

    zoomIn() {
        var num = this.state.cellHeight + 15
        this.setState({ cellHeight: num });
    }

    zoomOut() {
        var num = this.state.cellHeight - 15
        this.setState({ cellHeight: num });
    }

    handleDateRangeChange(startDate, endDate) {
        this.setState({ startDate: startDate });
    }

    handleRangeSelection(selected) {
        this.setState({ selected: selected, showCtrl: true });
        this._openModal();
    }

    _openModal() {
        this.setState({ showModal: true });
    }

    _closeModal(e) {
        if (e) {
            e.stopPropagation();
            e.preventDefault();
        }

        this.setState({ showModal: false });
    }

    handleItemChange(items, item) {
        this.updateEvent(items, item);
    }

    handleItemSize(items, item) {
        this.updateEvent(items, item);
    }

    removeEvent(items, item) {
        const eventStartYear = item.startDateTime.getFullYear();
        const eventStartMonth = item.startDateTime.getMonth();
        const eventStartDay = item.startDateTime.getDate();
        const eventStartHour = item.startDateTime.getHours();
        const eventStartMinutes = item.startDateTime.getMinutes();
        const eventEndYear = item.endDateTime.getFullYear();
        const eventEndMonth = item.endDateTime.getMonth();
        const eventEndDay = item.endDateTime.getDate();
        const eventEndHour = item.endDateTime.getHours();
        const eventEndMinutes = item.endDateTime.getMinutes();

        axios.post('Agenda/RemoveEvent', {
            Id: item._id,
            UserName: this.props.location.state.userName,
            EventName: item.name,
            EventColor: item.classes,
            StartYear: eventStartYear.toString(),
            StartMonth: eventStartMonth.toString(),
            StartDay: eventStartDay.toString(),
            StartHour: eventStartHour.toString(),
            StartMinutes: eventStartMinutes.toString(),
            EndYear: eventEndYear.toString(),
            EndMonth: eventEndMonth.toString(),
            EndDay: eventEndDay.toString(),
            EndHour: eventEndHour.toString(),
            EndMinutes: eventEndMinutes.toString()
        }).then((success) => {
            this.setState({
                message: success.data,
                show: true,
                isSuccess: true,
                showModal: false,
                items: items
            });
        }).catch((error) => {
            this.setState({
                show: true,
                isSuccess: false,
                message: "Server could not be contacted, try in some minutes"
            });
        });
    }

    addNewEvent(items, newItems) {
        const eventStartYear = newItems.startDateTime.getFullYear();
        const eventStartMonth = newItems.startDateTime.getMonth();
        const eventStartDay = newItems.startDateTime.getDate();
        const eventStartHour = newItems.startDateTime.getHours();
        const eventStartMinutes = newItems.startDateTime.getMinutes();
        const eventEndYear = newItems.endDateTime.getFullYear();
        const eventEndMonth = newItems.endDateTime.getMonth();
        const eventEndDay = newItems.endDateTime.getDate();
        const eventEndHour = newItems.endDateTime.getHours();
        const eventEndMinutes = newItems.endDateTime.getMinutes();

        axios.post('Agenda/RegisterEvent', {
            UserName: this.props.location.state.userName,
            EventName: newItems.name,
            EventColor: newItems.classes,
            StartYear: eventStartYear.toString(),
            StartMonth: eventStartMonth.toString(),
            StartDay: eventStartDay.toString(),
            StartHour: eventStartHour.toString(),
            StartMinutes: eventStartMinutes.toString(),
            EndYear: eventEndYear.toString(),
            EndMonth: eventEndMonth.toString(),
            EndDay: eventEndDay.toString(),
            EndHour: eventEndHour.toString(),
            EndMinutes: eventEndMinutes.toString()
        }).then((success) => {
            this.getEvents();

            this.setState({
                message: success.data,
                show: true,
                isSuccess: true,
                showModal: false,
                selected: []
            });
        }).catch((error) => {
            this.getEvents();

            let errorMessage = "Server could not be contacted, try in some minutes";

            if (error.response.data.title === "One or more validation errors occurred.") {
                errorMessage = "Event name is invalid, characters allowed only.";
            }

            this.setState({
                show: true,
                isSuccess: false,
                message: errorMessage
            });
        });

        this._closeModal();
    }

    editEvent(items, item) {
        this.updateEvent(items, item);
        this._closeModal();
    }

    changeView(days, event) {
        this.setState({ numberOfDays: days });
    }

    hideModal() {
        this.setState({ show: false });
    };

    getEvents() {
        const items = [];
        this.setState({ items: [] });

        axios.post('Agenda/UserEvents', {
            UserName: this.props.location.state.userName
        }).then((success) => {
            success.data.forEach((event) => {
                items.push({
                    _id: event.id,
                    name: event.eventName,
                    startDateTime: new Date(Number(event.startYear), Number(event.startMonth),
                        Number(event.startDay), Number(event.startHour), Number(event.startMinutes)),
                    endDateTime: new Date(Number(event.endYear), Number(event.endMonth),
                        Number(event.endDay), Number(event.endHour), Number(event.endMinutes)),
                    classes: event.eventColor
                })
            });

            this.setState({ items: items });
        }).catch((error) => {
            this.setState({
                show: true,
                isSuccess: false,
                message: "Server could not be contacted, try in some minutes"
            });
        });
    }

    updateEvent(items, item) {
        const eventStartYear = item.startDateTime.getFullYear();
        const eventStartMonth = item.startDateTime.getMonth();
        const eventStartDay = item.startDateTime.getDate();
        const eventStartHour = item.startDateTime.getHours();
        const eventStartMinutes = item.startDateTime.getMinutes();
        const eventEndYear = item.endDateTime.getFullYear();
        const eventEndMonth = item.endDateTime.getMonth();
        const eventEndDay = item.endDateTime.getDate();
        const eventEndHour = item.endDateTime.getHours();
        const eventEndMinutes = item.endDateTime.getMinutes();

        axios.post('Agenda/UpdateEvent', {
            Id: item._id,
            UserName: this.props.location.state.userName,
            EventName: item.name,
            EventColor: item.classes,
            StartYear: eventStartYear.toString(),
            StartMonth: eventStartMonth.toString(),
            StartDay: eventStartDay.toString(),
            StartHour: eventStartHour.toString(),
            StartMinutes: eventStartMinutes.toString(),
            EndYear: eventEndYear.toString(),
            EndMonth: eventEndMonth.toString(),
            EndDay: eventEndDay.toString(),
            EndHour: eventEndHour.toString(),
            EndMinutes: eventEndMinutes.toString()
        }).then((success) => {
            this.setState({
                message: success.data,
                show: true,
                isSuccess: true,
                showModal: false,
                selected: [],
                items: items
            });
        }).catch((error) => {
            this.getEvents();

            let errorMessage = "Server could not be contacted, try in some minutes";

            if (error.response.data.title === "One or more validation errors occurred.") {
                errorMessage = "Event name is invalid, characters allowed only.";
            }

            this.setState({
                show: true,
                isSuccess: false,
                message: errorMessage
            });
        });
    }

    render() {
        var AgendaItem = function (props) {
            console.log(' item component props', props)
            return <div style={{ display: 'block', position: 'absolute', background: '#FFF' }}>{props.item.name} <button onClick={() => props.edit(props.item)}>Edit </button></div>
        }

        return (
            <div className="content-expanded ">
                <div className="control-buttons">
                    <button className="button-control" onClick={this.zoomIn}> <i className="zoom-plus-icon"></i> </button>
                    <button className="button-control" onClick={this.zoomOut}> <i className="zoom-minus-icon"></i> </button>
                    <button className="button-control" onClick={this._openModal}> <i className="schedule-icon"></i> </button>
                    <button className="button-control" onClick={this.changeView.bind(null, 7)}> {moment.duration(7, "days").humanize()}  </button>
                    <button className="button-control" onClick={this.changeView.bind(null, 4)}> {moment.duration(4, "days").humanize()}  </button>
                    <button className="button-control" onClick={this.changeView.bind(null, 3)}> {moment.duration(3, "days").humanize()}  </button>
                    <button className="button-control" onClick={this.changeView.bind(null, 1)}> {moment.duration(1, "day").humanize()} </button>
                    <a href="/" className="logout-link">
                        Logout
                    </a>
                </div>
                <ReactAgenda
                    minDate={new Date(now.getFullYear(), now.getMonth() - 3)}
                    maxDate={new Date(now.getFullYear(), now.getMonth() + 3)}
                    startDate={this.state.startDate}
                    startAtTime={8}
                    endAtTime={23}
                    cellHeight={this.state.cellHeight}
                    locale="en-gb"
                    items={this.state.items}
                    numberOfDays={this.state.numberOfDays}
                    headFormat={"ddd DD MMM"}
                    rowsPerHour={this.state.rowsPerHour}
                    itemColors={colors}
                    helper={true}
                    view="calendar"
                    autoScale={false}
                    fixedHeader={true}
                    onRangeSelection={this.handleRangeSelection.bind(this)}
                    onChangeEvent={this.handleItemChange.bind(this)}
                    onChangeDuration={this.handleItemSize.bind(this)}
                    onItemEdit={this.handleItemEdit.bind(this)}
                    onCellSelect={this.handleCellSelection.bind(this)}
                    onItemRemove={this.removeEvent.bind(this)}
                    onDateRangeChange={this.handleDateRangeChange.bind(this)} />
                    {
                        this.state.showModal ? <Modal clickOutside={this._closeModal} >
                            <div className="modal-content">
                                <ReactAgendaCtrl items={this.state.items} itemColors={colors} selectedCells={this.state.selected} Addnew={this.addNewEvent} edit={this.editEvent} />
                            </div>
                        </Modal> : ''
                    }
                    {   this.state.show &&
                        <MessageModal handleClose={this.hideModal} isSuccess={this.state.isSuccess} fromAgenda={true}>
                            <p className="modal-text">
                                {this.state.message}
                            </p>
                        </MessageModal>
                    }
            </div>
        );
    }
}