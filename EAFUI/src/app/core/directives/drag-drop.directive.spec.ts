/* tslint:disable:no-unused-variable */
import { TestBed, ComponentFixture } from '@angular/core/testing'
import { Component, DebugElement } from '@angular/core'
import { By } from '@angular/platform-browser'
import { DragDropDirective } from './drag-drop.directive'
import { ChangeDetectionStrategy } from '@angular/core'

@Component({
    // template: `<input type="text" style="background-color: red;">`
    template: `<div appDragDrop style="border-color: rgba(0, 0, 0, 0.38); opacity: 1; color: rgba(0, 0, 0, 0.38);">`
})

class TestDragDropComponent {}

const ivantiRed = 'rgb(218, 41, 28)'
const grey = 'rgba(0, 0, 0, 0.38)'

describe('Directive: DragDrop', () => {

    let component: TestDragDropComponent
    let fixture: ComponentFixture<TestDragDropComponent>
    let debugElement: HTMLElement
    let div: HTMLElement
    let divEl: DebugElement

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [TestDragDropComponent, DragDropDirective]
        }).overrideComponent(TestDragDropComponent, {
            set: { changeDetection: ChangeDetectionStrategy.Default }
        })
        .compileComponents()
    })

    beforeEach(() => {
        fixture = TestBed.createComponent(TestDragDropComponent)
        component = fixture.componentInstance
        debugElement = fixture.debugElement.nativeElement
        div = debugElement.querySelector('div')
        divEl = fixture.debugElement.query(By.css('div'))
        fixture.detectChanges()
    })

    it('drag over', () => {
        // Arrange

        // Act
        divEl.triggerEventHandler('dragover', new DragEvent('dragover'))
        fixture.detectChanges()

        // Assert
        expect(divEl.nativeElement.style.borderColor).toBe(ivantiRed)
        expect(divEl.nativeElement.style.color).toBe(ivantiRed)
        expect(divEl.nativeElement.style.opacity).toBe('0.8')
    })

    it('drag leave', () => {
        // Arrange
        // Act
        divEl.triggerEventHandler('dragleave', new DragEvent('dragleave'))
        fixture.detectChanges()

        // Assert
        expect(divEl.nativeElement.style.borderColor).toBe(grey)
        expect(divEl.nativeElement.style.color).toBe(grey)
        expect(divEl.nativeElement.style.opacity).toBe('1')
    })

    it('drop', () => {
        // Arrange
        const dragEvent = new DragEvent('drop')
        const mockEvent = {
            ...dragEvent,
            dataTransfer: new DataTransfer(),
        } as DragEvent

        // Act
        divEl.triggerEventHandler('drop', mockEvent)
        fixture.detectChanges()

        // Assert
        expect(divEl.nativeElement.style.borderColor).toBe(grey)
        expect(divEl.nativeElement.style.color).toBe(grey)
        expect(divEl.nativeElement.style.opacity).toBe('1')
    })
})
