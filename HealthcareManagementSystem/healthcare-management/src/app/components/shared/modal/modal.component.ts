// src/app/components/shared/modal/modal.component.ts
import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-modal',
  imports:[CommonModule],
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent {
  @Input() title: string = 'Modal Title';
  @Input() isVisible: boolean = false;

  @Output() closeModal: EventEmitter<void> = new EventEmitter<void>();

  constructor() {}

  close() {
    this.closeModal.emit();
  }
}
