import { Expression } from "../Abstract/Expression";
import { Instruction } from "../Abstract/Instruction";
import { Type } from "../Abstract/Return";
import { Environment } from "../Symbols/Environment";

export class If extends Instruction {
  constructor(
    private condition: Expression,
    private instructions: Instruction[],
    private elseInstructions: Instruction[],
    line: number,
    column: number
  ) {
    super(line, column);
  }

  public execute(environment: Environment) {
    const condition = this.condition.execute(environment);
    if (condition.type !== Type.BOOLEAN)
      throw new Error(`Condition must be boolean`);

    if (condition.value) {
      for (const instruction of this.instructions) {
        instruction.execute(environment);
      }
    } else {
      for (const instruction of this.elseInstructions) {
        instruction.execute(environment);
      }
    }
  }
}
